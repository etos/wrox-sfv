/*
    MTK Software - WRoX-SFV
    Copyright (C) 2008  Daniel Stephenson

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/


/*  Author: Daniel Stephenson
    Date: 27/03/05
    Filename: SafeInvoke.cs
    Function: main gui to wrox-sfv
*/

using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace MTK.WRoX_SFV
{

    public class SafeInvoke
    {
        static readonly ModuleBuilder builder;
        static readonly AssemblyBuilder myAsmBuilder;
        static readonly Hashtable methodLookup;

        static SafeInvoke()
        {
            AssemblyName name = new AssemblyName();
            name.Name = "temp";
            myAsmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
            builder = myAsmBuilder.DefineDynamicModule("TempModule");
            methodLookup = new Hashtable();
        }

        public static object Invoke(System.Windows.Forms.Control obj, string methodName, params object[] paramValues)
        {
            Delegate del = null;
            string key = obj.GetType().Name + "." + methodName;
            Type tp;
            lock (methodLookup)
            {
                if (methodLookup.Contains(key))
                    tp = (Type)methodLookup[key];
                else
                {
                    Type[] paramList = new Type[obj.GetType().GetMethod(methodName).GetParameters().Length];
                    int n = 0;
                    foreach (ParameterInfo pi in obj.GetType().GetMethod(methodName).GetParameters()) paramList[n++] = pi.ParameterType;
                    TypeBuilder typeB = builder.DefineType("Del_" + obj.GetType().Name + "_" + methodName, TypeAttributes.Class | TypeAttributes.AutoLayout | TypeAttributes.Public | TypeAttributes.Sealed, typeof(MulticastDelegate), PackingSize.Unspecified);
                    ConstructorBuilder conB = typeB.DefineConstructor(MethodAttributes.HideBySig | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName, CallingConventions.Standard, new Type[] { typeof(object), typeof(IntPtr) });
                    conB.SetImplementationFlags(MethodImplAttributes.Runtime);
                    MethodBuilder mb = typeB.DefineMethod("Invoke", MethodAttributes.Public | MethodAttributes.Virtual | MethodAttributes.HideBySig, obj.GetType().GetMethod(methodName).ReturnType, paramList);
                    mb.SetImplementationFlags(MethodImplAttributes.Runtime);
                    tp = typeB.CreateType();
                    methodLookup.Add(key, tp);
                }
            }

            del = MulticastDelegate.CreateDelegate(tp, obj, methodName);
            return obj.Invoke(del, paramValues);
        }
    }
}