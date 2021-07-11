using System;
using System.Linq;
using UnityEngine;

namespace GameUtils
{
    public static class GenericTypeHelper
    {
        /// <summary>
        /// �ж�ָ�������� <paramref name="type"/> �Ƿ���ָ���������͵������ͣ���ʵ����ָ�����ͽӿڡ�
        /// </summary>
        /// <param name="type">��Ҫ���Ե����͡�</param>
        /// <param name="generic">���ͽӿ����ͣ����� typeof(IXxx&lt;&gt;)</param>
        /// <returns>����Ƿ��ͽӿڵ������ͣ��򷵻� true�����򷵻� false��</returns>
        public static bool HasImplementedRawGeneric(this Type type, Type generic)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (generic == null) throw new ArgumentNullException(nameof(generic));

            // ���Խӿڡ�
            var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);
            if (isTheRawGenericType) return true;

            // �������͡�
            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }

            // û���ҵ��κ�ƥ��Ľӿڻ����͡�
            return false;

            // ����ĳ�������Ƿ���ָ����ԭʼ�ӿڡ�
            bool IsTheRawGenericType(Type test)
                => generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
        }
    }
}