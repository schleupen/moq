// Copyright (c) 2007, Clarius Consulting, Manas Technology Solutions, InSTEDD.
// All rights reserved. Licensed under the BSD 3-Clause License; see License.txt.

using System;
using System.Linq.Expressions;

namespace Moq
{
	/// <summary>
	///   A setup configured on a mock.
	/// </summary>
	/// <seealso cref="Mock.Setups"/>
	public interface ISetup
	{
		/// <summary>
		///   The setup expression.
		/// </summary>
		LambdaExpression Expression { get; }

		/// <summary>
		///   Gets the inner mock of this setup (if present and known).
		///   <para>
		///     An "inner mock" is the <see cref="Moq.Mock"/> instance associated with a setup's return value,
		///     if that setup is configured to return a mock object.
		///   </para>
		///   <para>
		///     This property will be <see langword="null"/> if a setup either does not return a mock object,
		///     or if Moq cannot safely determine its return value without risking any side effects. For instance,
		///     Moq is able to inspect the return value if it is a constant (e.g. <c>`.Returns(value)`</c>);
		///     if, on the other hand, it gets computed by a factory function (e.g. <c>`.Returns(() => value)`</c>),
		///     Moq will not attempt to retrieve that value just to find the inner mock,
		///     since calling a user-provided function could have effects beyond Moq's understanding and control.
		///   </para>
		/// </summary>
		Mock InnerMock { get; }

		/// <summary>
		///   Gets whether this setup is conditional.
		/// </summary>
		/// <seealso cref="Mock{T}.When(Func{bool})"/>
		bool IsConditional { get; }

		/// <summary>
		///   Gets whether this setup has been overridden
		///   (that is, whether it is being shadowed by a more recent non-conditional setup with an equal expression).
		/// </summary>
		bool IsOverridden { get; }

		/// <summary>
		///   Gets whether this setup is "verifiable".
		/// </summary>
		/// <remarks>
		///   This property gets sets by the <c>`.Verifiable()`</c> setup verb.
		///   <para>
		///     Note that setups can be verified even if this property is <see langword="false"/>:
		///     <see cref="Mock.VerifyAll()"/> completely ignores this property.
		///     <see cref="Mock.Verify()"/>, however, will only verify setups where this property is <see langword="true"/>.
		///   </para>
		/// </remarks>
		bool IsVerifiable { get; }

		/// <summary>
		///   Returns the <see cref="Mock"/> instance to which this setup belongs.
		/// </summary>
		Mock Mock { get; }

		/// <summary>
		///   Returns the original setup as it would appear in user code.
		///   <para>
		///     For setups doing a simple member access or method invocation (such as <c>`mock => mock.Member`</c>),
		///     this property will simply return the same <see cref="ISetup"/> instance.
		///   </para>
		///   <para>
		///     For setups whose expression involves member chaining (such as <c>`parent => parent.Child.Member`</c>),
		///     Moq does not create a single setup, but one for each member access/invocation.
		///     The example just given will result in two setups:
		///     <list type="number">
		///       <item>a setup for <c>`parent => parent.Child`</c> on the parent mock; and</item>
		///       <item>on its inner mock, a setup for <c>`(child) => (child).Member`</c>.</item>
		///     </list>
		///     These are the setups that will be put in the mocks' <see cref="Mock.Setups"/> collections;
		///     for both of those, <see cref="OriginalSetup"/> will return the same <see cref="IFluentSetup"/>
		///     whose <see cref="ISetup.Expression"/> represents the original, fluent setup expression.
		///   </para>
		/// </summary>
		/// <seealso cref="IFluentSetup"/>
		ISetup OriginalSetup { get; }

		/// <summary>
		///   Gets whether this setup was matched by at least one invocation on the mock.
		/// </summary>
		bool WasMatched { get; }

		/// <summary>
		///   Verifies this setup and optionally all verifiable setups of its inner mock (if present and known).
		///   <para>
		///     If <paramref name="recursive"/> is set to <see langword="true"/>,
		///     the semantics of this method are essentially the same as those of <see cref="Mock.Verify()"/>,
		///     except that this setup (instead of a mock) is used as the starting point for verification,
		///     and will always be verified itself (even if not flagged as verifiable).
		///   </para>
		/// </summary>
		/// <param name="recursive">
		///   Specifies whether recursive verification should be performed.
		/// </param>
		/// <exception cref="MockException">
		///   Verification failed due to one or more unmatched setups.
		/// </exception>
		/// <seealso cref="VerifyAll()"/>
		/// <seealso cref="Mock.Verify()"/>
		void Verify(bool recursive = true);

		/// <summary>
		///   Verifies this setup and all setups of its inner mock (if present and known),
		///   regardless of whether they have been flagged as verifiable.
		///   <para>
		///     The semantics of this method are essentially the same as those of <see cref="Mock.VerifyAll()"/>,
		///     except that this setup (instead of a mock) is used as the starting point for verification.
		///   </para>
		/// </summary>
		/// <exception cref="MockException">
		///   Verification failed due to one or more unmatched setups.
		/// </exception>
		/// <seealso cref="Verify(bool)"/>
		/// <seealso cref="Mock.VerifyAll()"/>
		void VerifyAll();
	}
}
