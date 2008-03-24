using System;
using System.Collections;
using System.Text;
using Creshendo.Util.Collections;
using Creshendo.Util.Rete.Exception;

namespace Creshendo.Util.Rete
{
    public class TemporalEqNode : AbstractTemporalNode
    {
        public TemporalEqNode(int id) : base(id)
        {
        }

        public override void assertLeft(Index linx, Rete engine, IWorkingMemory mem)
        {
            long time = RightTime;
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            leftmem.Put(linx, linx);
            EqHashIndex inx = new EqHashIndex(NodeUtils.getLeftValues(binds, linx.Facts));
            TemporalHashedAlphaMem rightmem = (TemporalHashedAlphaMem) mem.getBetaRightMemory(this);
            IEnumerator itr = rightmem.iterator(inx);
            if (itr != null)
            {
                try
                {
                    while (itr.MoveNext())
                    {
                        IFact vl = (IFact) itr.Current;
                        if (vl != null)
                        {
                            if (vl.timeStamp() > time)
                            {
                                propogateAssert(linx.add(vl), engine, mem);
                            }
                            else
                            {
                                rightmem.removePartialMatch(inx, vl);
                                propogateRetract(linx.add(vl), engine, mem);
                            }
                        }
                    }
                }
                catch (RetractException e)
                {
                    // there shouldn't be any retract exceptions
                }
            }
        }

        public override void assertRight(IFact rfact, Rete engine, IWorkingMemory mem)
        {
            long time = LeftTime;
            TemporalHashedAlphaMem rightmem = (TemporalHashedAlphaMem) mem.getBetaRightMemory(this);
            EqHashIndex inx = new EqHashIndex(NodeUtils.getRightValues(binds, rfact));
            rightmem.addPartialMatch(inx, rfact);
            // now that we've added the facts to the list, we
            // proceed with evaluating the fact
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            // since there may be key collisions, we iterate over the
            // values of the HashMap. If we used keySet to iterate,
            // we could encounter a ClassCastException in the case of
            // key collision.
            IEnumerator itr = leftmem.Values.GetEnumerator();
            try
            {
                while (itr.MoveNext())
                {
                    Index linx = (Index) itr.Current;
                    if (evaluate(linx.Facts, rfact, time))
                    {
                        // now we propogate
                        propogateAssert(linx.add(rfact), engine, mem);
                    }
                    else
                    {
                        propogateRetract(linx.add(rfact), engine, mem);
                    }
                }
            }
            catch (RetractException e)
            {
                // we shouldn't Get a retract exception. if we do, it's a bug
            }
        }

        public override void retractLeft(Index linx, Rete engine, IWorkingMemory mem)
        {
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            leftmem.Remove(linx);
            EqHashIndex eqinx = new EqHashIndex(NodeUtils.getLeftValues(binds, linx.Facts));
            TemporalHashedAlphaMem rightmem = (TemporalHashedAlphaMem) mem.getBetaRightMemory(this);

            // now we propogate the retract. To do that, we have
            // merge each item in the list with the Fact array
            // and call retract in the successor nodes
            IEnumerator itr = rightmem.iterator(eqinx);
            if (itr != null)
            {
                while (itr.MoveNext())
                {
                    propogateRetract(linx.add((IFact) itr.Current), engine, mem);
                }
            }
        }

        public override void retractRight(IFact rfact, Rete engine, IWorkingMemory mem)
        {
            long time = LeftTime;
            EqHashIndex inx = new EqHashIndex(NodeUtils.getRightValues(binds, rfact));
            TemporalHashedAlphaMem rightmem = (TemporalHashedAlphaMem) mem.getBetaRightMemory(this);
            // first we Remove the fact from the right
            rightmem.removePartialMatch(inx, rfact);
            // now we see the left memory matched and Remove it also
            IGenericMap<Object, Object> leftmem = (IGenericMap<Object, Object>) mem.getBetaLeftMemory(this);
            IEnumerator itr = leftmem.Values.GetEnumerator();
            while (itr.MoveNext())
            {
                Index linx = (Index) itr.Current;
                if (evaluate(linx.Facts, rfact, time))
                {
                    propogateRetract(linx.add(rfact), engine, mem);
                }
            }
        }

        public override String toPPString()
        {
            StringBuilder buf = new StringBuilder();
            for (int idx = 0; idx < binds.Length; idx++)
            {
                if (idx > 0)
                {
                    buf.Append(" && ");
                }
                buf.Append(binds[idx].toBindString());
            }
            return buf.ToString();
        }

        public override String ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("TemporalEqNode-" + nodeID + "> ");
            buf.Append("left=" + leftElapsedTime + " , right=" + rightElapsedTime + " - ");
            for (int idx = 0; idx < binds.Length; idx++)
            {
                if (idx > 0)
                {
                    buf.Append(" && ");
                }
                if (binds[idx] != null)
                {
                    buf.Append(binds[idx].toPPString());
                }
            }
            return buf.ToString();
        }
    }
}