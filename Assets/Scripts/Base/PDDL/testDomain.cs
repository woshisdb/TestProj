using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
/// <summary>
/// DomainµÄ¾²Ì¬º¯Êý
/// </summary>
public class DomainProblemStatic
{
    public static List<PType> GetPType()
    {
        var ret=new List<PType>();
        return ret;
    }
    public static List<Predicate> GetPredics()
    {
        var ret=new List<Predicate>();
        ret.Add(P.Is(new PType(), new PType()).predicate);
        ret.Add(new Predicate("HasMap",new TableModelType(),new TableModelType()));
        return ret;
    }
    public static List<Func> GetFuncs()
    {
        var ret = new List<Func>();
        ret.Add(new Func("MapLen", new TableModelType(), new TableModelType()));
        return ret;
    }
    public static List<PType> GetObjs()
    {
        var ret=new List<PType>();
        return ret;
    }
    public static List<Bool> GetPredVals()
    {
        var ret=new List<Bool>();
        ret.AddRange(GameArchitect.get.objAsset.map.GetPredVals());
        return ret;
    }
    public static List<Num> GetNums()
    {
        var ret= new List<Num>();
        ret.AddRange(GameArchitect.get.objAsset.map.GetFuncVals());
        return ret;
    }

}
public class Domain
{
    protected class MyCustomComparer : IEqualityComparer<PType>
    {
        public bool Equals(PType x, PType y)
        {
            if (x is null || y is null)
                return false;
            return x.typeName == y.typeName;
        }

        public int GetHashCode(PType obj)
        {
            return obj.typeName.GetHashCode();
        }
    }

    protected class FuncComparer : IEqualityComparer<Func>
    {
        public bool Equals(Func x, Func y)
        {
            if (x is null || y is null)
                return false;
            return x.name == y.name;
        }

        public int GetHashCode(Func obj)
        {
            return obj.name.GetHashCode();
        }
    }

    protected class PredicateComparer : IEqualityComparer<Predicate>
    {
        public bool Equals(Predicate x, Predicate y)
        {
            if (x is null || y is null)
                return false;
            return x.name == y.name;
        }

        public int GetHashCode(Predicate obj)
        {
            return obj.name.GetHashCode();
        }
    }

    protected class PActionComparer : IEqualityComparer<PAction>
    {
        public bool Equals(PAction x, PAction y)
        {
            if (x is null || y is null)
                return false;
            return x.actionName == y.actionName;
        }

        public int GetHashCode(PAction obj)
        {
            return obj.actionName.GetHashCode();
        }
    }

    public HashSet<PType> pTypes;
    public HashSet<Predicate> predicates;
    public HashSet<Func> funcs;
    public HashSet<PAction> pActions;
    public string domainName;
    public Domain()
    {
        domainName = "TestDomain";
        pTypes = new HashSet<PType>(new MyCustomComparer());
        predicates = new HashSet<Predicate>(new PredicateComparer());
        funcs = new HashSet<Func>(new FuncComparer());
        pActions = new HashSet<PAction>(new PActionComparer());
        predicates.UnionWith(DomainProblemStatic.GetPredics());
        funcs.UnionWith(DomainProblemStatic.GetFuncs());
    }
    public string Print()
    {
        StringBuilder str = new StringBuilder();
        str.AppendFormat("(define (domain {0})\n", domainName);
        str.AppendLine("(:requirements :strips :typing :durative-actions)");
        if (pTypes.Count > 0)
        {
            str.AppendLine("(:types ");
            str.AppendLine("PType");
            foreach (var p in pTypes)
            {
                str.AppendFormat("{0}-{1}\n", p.typeName, p.GetType().BaseType.Name);
            }
            str.AppendLine("\n)\n");
        }
        if (predicates.Count > 0)
        {
            str.AppendLine("(:predicates\n");

            foreach (var p in predicates)
            {
                p.f(str);
                str.Append("\n");
            }
            str.AppendLine("\n)");
        }
        if (funcs.Count > 0)
        {
            str.AppendLine("(:functions\n");

            foreach (var p in funcs)
            {
                str.AppendLine(p.ToString() + " ");
            }
            str.AppendLine("\n)");
        }

        foreach (var p in pActions)
        {
            str.AppendLine(p.ToString());
        }
        str.AppendLine(")");
        //for (int i = 0; i < axioms.Count; i++)
        //{
        //    str.AppendLine(axioms[i].ToString());
        //}
        return str.ToString();
    }
    public void AddTypes(List<PType> ps)
    {
        if (ps != null)
        {
            foreach (var x in ps)
            {
                AddType(x);
               //var has= pTypes.Contains(x);
               // if(!has)
               // {
               //     pTypes.Add(x);
               // }
            }
        }
    }
    public void AddType(PType x)
    {
        var has = pTypes.Contains(x);
        if (!has)
        {
            pTypes.Add(x);
        }
    }
    public void AddFuncs(List<Func> ps)
    {
        if (ps != null)
        {
            foreach (var x in ps)
            {
                var has = funcs.Contains(x);
                if (!has)
                {
                    funcs.Add(x);
                }
            }
        }
    }

    public void AddPreds(List<Predicate> ps)
    {
        if (ps != null)
        {
            foreach (var x in ps)
            {
                var has = predicates.Contains(x);
                if (!has)
                {
                    predicates.Add(x);
                }
            }
        }
    }
}