
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Models
{
    /// <summary>
    /// 用于保存
    /// </summary>
    public class DtoSave<T>
    {
        public string token { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        public DtoSave()
        {
            saveFieldList = new List<string>();
            ignoreFieldList = new List<string>();
        }
        /// <summary>
        /// 用于操作的对象
        /// </summary>
        public T data { get; set; }
        /// <summary>
        /// 需要保存的字段
        /// </summary>
        private List<string> _saveFieldList;
        /// <summary>
        /// 需要忽略的字段
        /// </summary>
        private List<string> _ignoreFieldList;
        /// <summary>
        /// 更新条件,如果为空.则以主键为更新条件
        /// </summary>
        private List<string> _whereList;

        /// <summary>
        /// 
        /// </summary>
        public Expression<Func<T, object[]>> saveFieldListExp { get; set; }
        public Expression<Func<T, object[]>> ignoreFieldListExp { get; set; }
        public Expression<Func<T, object[]>> whereListExp { get; set; }
        public List<string> saveFieldList
        {
            get {
                if ((_saveFieldList == null || _saveFieldList.Count==0) && saveFieldListExp!=null) {
                    _saveFieldList = LamdbToList(saveFieldListExp);
                }
                return _saveFieldList; 
            }
            set => _saveFieldList = value;
        }

        public List<string> ignoreFieldList
        {
            get
            {
                if ((_ignoreFieldList == null || _ignoreFieldList.Count == 0) && ignoreFieldListExp != null)
                {
                    _ignoreFieldList = LamdbToList(ignoreFieldListExp);
                }
                return _ignoreFieldList;
            }
            set => _ignoreFieldList = value;
        }

        public List<string> whereList { 
            get
            {
                if ((_whereList == null || _whereList.Count == 0) && whereListExp != null)
                {
                    _whereList = LamdbToList(whereListExp);
                }
                return _whereList;
            }
            set => _whereList = value; 
        }




        /// <summary>
        /// 将查询表达式。转成数据,用于获取保存字段使用
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        private List<string> LamdbToList<T1>(Expression<Func<T1, object[]>> expression)
        {
            List<string> reList = new List<string>();

            switch (expression.Body.NodeType)
            {
                case ExpressionType.NewArrayInit:
                    NewArrayExpression bodyExp = (NewArrayExpression)expression.Body;

                    foreach (var item in bodyExp.Expressions)
                    {
                        switch (item.NodeType)
                        {
                            case ExpressionType.MemberAccess:
                                MemberExpression itemBody = (MemberExpression)item;
                                reList.Add(itemBody.Member.Name);
                                break;
                            case ExpressionType.Convert:
                                UnaryExpression itemBody1 = (UnaryExpression)item;
                                if (itemBody1.Operand.NodeType == ExpressionType.MemberAccess)
                                {
                                    MemberExpression itemBody2 = (MemberExpression)itemBody1.Operand;
                                    reList.Add(itemBody2.Member.Name);
                                }
                                break;
                        }
                    }
                    break;
                default:
                    break;
            }
            return reList;
        }
    }
}