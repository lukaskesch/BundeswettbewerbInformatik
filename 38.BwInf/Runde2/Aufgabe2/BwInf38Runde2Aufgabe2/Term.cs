using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BwInf38Runde2Aufgabe2
{
    public abstract class Term
    {
        private static double LogLong = Math.Log10(long.MaxValue);
        private static double LogLogLong = Math.Log10(Math.Log10(long.MaxValue));

        public virtual long GetResult()
        {
            return 0;
        }
        public virtual string PrintTerm()
        {
            return string.Empty;
        }
        public static double GetLogLong()
        {
            return LogLong;
        }
        public static double GetLogLogLong()
        {
            return LogLogLong;
        }
    }
    public class Literal : Term
    {
        long Value;
        public Literal(long _Value) : base()
        {
            Value = _Value;
        }

        public override long GetResult()
        {
            return Value;
        }
        public override string PrintTerm()
        {
            return Value.ToString();
        }
    }
    public class FactorialOperator : Term
    {
        protected long result;
        protected Term Term1;
        public FactorialOperator(Term PriorTerm) : base()
        {
            Term1 = PriorTerm;
            result = 1;
            long Number = PriorTerm.GetResult();
            while (Number != 1)
            {
                result *= Number;
                Number--;
            }
        }

        public override long GetResult()
        {
            return result;
        }
        public override string PrintTerm()
        {
            return "(" + Term1.PrintTerm() + ")!";
        }
        public static bool IsCalculatable(Term Term1)
        {
            if (Term1.GetResult() <= 20)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class AddOperator : Term
    {
        long result;
        protected Term Term1;
        protected Term Term2;
        public AddOperator(Term _Term1, Term _Term2)
        {
            Term1 = _Term1;
            Term2 = _Term2;
            result = Term1.GetResult() + Term2.GetResult();
        }

        public override long GetResult()
        {
            return result;
        }
        public override string PrintTerm()
        {
            return "(" + Term1.PrintTerm() + "+" + Term2.PrintTerm() + ")";
        }
        public static bool IsCalculatable(Term Term1, Term Term2)
        {
            long Check = long.MaxValue;
            Check -= Term1.GetResult() + Term2.GetResult();
            if (Check > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class SubtractOperator : Term
    {
        long result;
        protected Term Term1;
        protected Term Term2;
        public SubtractOperator(Term _Term1, Term _Term2)
        {
            Term1 = _Term1;
            Term2 = _Term2;
            result = Term1.GetResult() - Term2.GetResult();
        }

        public override long GetResult()
        {
            return result;
        }
        public override string PrintTerm()
        {
            return "(" + Term1.PrintTerm() + "-" + Term2.PrintTerm() + ")";
        }
    }

    public class MultiplyOperator : Term
    {
        long result;
        protected Term Term1;
        protected Term Term2;
        public MultiplyOperator(Term _Term1, Term _Term2)
        {
            Term1 = _Term1;
            Term2 = _Term2;
            result = Term1.GetResult() * Term2.GetResult();
        }

        public override long GetResult()
        {
            return result;
        }
        public override string PrintTerm()
        {
            return "(" + Term1.PrintTerm() + "*" + Term2.PrintTerm() + ")";
        }
        public static bool IsCalculatable(Term Term1, Term Term2)
        {
            if (GetLogLong() - 0.00000001 > Math.Log10(Term1.GetResult()) + Math.Log10(Term2.GetResult()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class DivisionOperator : Term
    {
        long result;
        protected Term Term1;
        protected Term Term2;
        public DivisionOperator(Term _Term1, Term _Term2)
        {
            Term1 = _Term1;
            Term2 = _Term2;
            result = Term1.GetResult() / Term2.GetResult();
        }

        public override long GetResult()
        {
            return result;
        }
        public override string PrintTerm()
        {
            return "(" + Term1.PrintTerm() + "/" + Term2.PrintTerm() + ")";
        }
        public static bool IsCalculatable(Term Term1, Term Term2)
        {
            int Remainder = (int)(Term1.GetResult() % Term2.GetResult());
            if (Remainder == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    public class ModuloOperator : Term
    {
        long result;
        protected Term Term1;
        protected Term Term2;
        public ModuloOperator(Term _Term1, Term _Term2)
        {
            Term1 = _Term1;
            Term2 = _Term2;
            result = Term1.GetResult() % Term2.GetResult();
        }

        public override long GetResult()
        {
            return result;
        }
        public override string PrintTerm()
        {
            return "(" + Term1.PrintTerm() + "%" + Term2.PrintTerm() + ")";
        }

    }

    public class PowerOperator : Term
    {
        long result;
        protected Term Term1;
        protected Term Term2;
        public PowerOperator(Term _Term1, Term _Term2)
        {
            Term1 = _Term1;
            Term2 = _Term2;
            result = (long)Math.Pow(Term1.GetResult(), Term2.GetResult());
        }

        public override long GetResult()
        {
            return result;
        }
        public override string PrintTerm()
        {
            return "(" + Term1.PrintTerm() + ")^(" + Term2.PrintTerm() + ")";
        }

        public static bool IsCalculatable(Term Term1, Term Term2)
        {
            if (GetLogLogLong() - 0.0000001 > Math.Log10(Term2.GetResult()) + Math.Log10(Math.Log10(Term1.GetResult())))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
