using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Bindable.Linq.Helpers;

namespace Bindable.Linq.Dependencies.ExpressionAnalysis
{
    public sealed class ExpressionFlattener
    {
	    private readonly List<Expression> _expressions = new List<Expression>();

	    private readonly ExpressionType[] _stopAt;

	    public IEnumerable<Expression> Expressions => _expressions;

	    public ExpressionFlattener(Expression expression, params ExpressionType[] stopAt)
	    {
		    _stopAt = stopAt;
		    TraverseExpression(expression);
	    }

	    private void TraverseExpressions(IEnumerable expressions)
        {
            if (expressions == null) return;
            foreach (Expression expression in expressions)
            {
                TraverseExpression(expression);
            }
        }

	    private void TraverseExpression(Expression expression)
        {
            if (expression == null) return;
            if (_stopAt.Contains(expression.NodeType))
            {
                _expressions.Add(expression);
            }
            else switch (expression)
            {
                case BinaryExpression binaryExpression:
                    TraverseBinaryExpression(binaryExpression);
                    break;
                case ConditionalExpression conditionalExpression:
                    TraverseConditionalExpression(conditionalExpression);
                    break;
                case ConstantExpression constantExpression:
                    TraverseConstantExpression(constantExpression);
                    break;
                case InvocationExpression invocationExpression:
                    TraverseInvocationExpression(invocationExpression);
                    break;
                case LambdaExpression lambdaExpression:
                    TraverseLambdaExpression(lambdaExpression);
                    break;
                case ListInitExpression initExpression:
                    TraverseListInitExpression(initExpression);
                    break;
                case MemberExpression memberExpression:
                    TraverseMemberExpression(memberExpression);
                    break;
                case MemberInitExpression memberInitExpression:
                    TraverseMemberInitExpression(memberInitExpression);
                    break;
                case MethodCallExpression callExpression:
                    TraverseMethodCallExpression(callExpression);
                    break;
                case NewArrayExpression arrayExpression:
                    TraverseNewArrayExpression(arrayExpression);
                    break;
                case NewExpression newExpression:
                    TraverseNewExpression(newExpression);
                    break;
                case ParameterExpression parameterExpression:
                    TraverseParameterExpression(parameterExpression);
                    break;
                case TypeBinaryExpression typeBinaryExpression:
                    TraverseTypeBinaryExpression(typeBinaryExpression);
                    break;
                case UnaryExpression unaryExpression:
                    TraverseUnaryExpression(unaryExpression);
                    break;
            }
        }

	    private void TraverseBinaryExpression(BinaryExpression binaryExpression)
	    {
		    TraverseExpression(binaryExpression.Conversion);
		    TraverseExpression(binaryExpression.Left);
		    TraverseExpression(binaryExpression.Right);
	    }

	    private void TraverseConditionalExpression(ConditionalExpression conditionalExpression)
	    {
		    TraverseExpression(conditionalExpression.IfFalse);
		    TraverseExpression(conditionalExpression.IfTrue);
		    TraverseExpression(conditionalExpression.Test);
	    }

	    private void TraverseConstantExpression(ConstantExpression constantExpression)
	    {
	    }

	    private void TraverseInvocationExpression(InvocationExpression invocationExpression)
	    {
		    TraverseExpressions(invocationExpression.Arguments);
		    TraverseExpression(invocationExpression.Expression);
	    }

	    private void TraverseLambdaExpression(LambdaExpression lambdaExpression)
	    {
		    TraverseExpressions(lambdaExpression.Parameters);
		    TraverseExpression(lambdaExpression.Body);
	    }

	    private void TraverseListInitExpression(ListInitExpression listInitExpression)
	    {
		    TraverseExpressions((from i in listInitExpression.Initializers
		    select i.Arguments.Cast<Expression>()).UnionAll());
		    TraverseExpression(listInitExpression.NewExpression);
	    }

	    private void TraverseMemberExpression(MemberExpression memberExpression)
	    {
		    TraverseExpression(memberExpression.Expression);
	    }

	    private void TraverseMemberInitExpression(MemberInitExpression memberInitExpression)
	    {
		    TraverseExpressions(from b in memberInitExpression.Bindings
		    where b.BindingType == MemberBindingType.Assignment
		    select ((MemberAssignment)b).Expression);
		    TraverseExpressions(from b in memberInitExpression.Bindings
		    where b.BindingType == MemberBindingType.ListBinding
		    select (from i in ((MemberListBinding)b).Initializers
		    select i.Arguments.Cast<Expression>()).UnionAll());
		    TraverseExpressions((from b in memberInitExpression.Bindings
		    where b.BindingType == MemberBindingType.MemberBinding
		    select ((MemberMemberBinding)b).Bindings.Cast<Expression>()).UnionAll());
		    TraverseExpression(memberInitExpression.NewExpression);
	    }

	    private void TraverseMethodCallExpression(MethodCallExpression methodCallExpression)
	    {
		    TraverseExpressions(methodCallExpression.Arguments);
		    TraverseExpression(methodCallExpression.Object);
	    }

	    private void TraverseNewArrayExpression(NewArrayExpression newArrayExpression)
	    {
		    TraverseExpressions(newArrayExpression.Expressions);
	    }

	    private void TraverseNewExpression(NewExpression newExpression)
	    {
		    TraverseExpressions(newExpression.Arguments);
	    }

	    private void TraverseParameterExpression(ParameterExpression parameterExpression)
	    {
	    }

	    private void TraverseTypeBinaryExpression(TypeBinaryExpression typeBinaryExpression)
	    {
		    TraverseExpression(typeBinaryExpression.Expression);
	    }

	    private void TraverseUnaryExpression(UnaryExpression unaryExpression)
	    {
		    TraverseExpression(unaryExpression.Operand);
	    }
    }

}