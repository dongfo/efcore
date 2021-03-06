// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.TestUtilities;
using Xunit;

namespace Microsoft.EntityFrameworkCore.Infrastructure
{
    public class InMemoryModelValidatorTest : ModelValidatorTestBase
    {
        [ConditionalFact]
        public virtual void Detects_ToQuery_on_derived_keyless_types()
        {
            var modelBuilder = base.CreateConventionalModelBuilder();
            var context = new DbContext(new DbContextOptions<DbContext>());
            InMemoryEntityTypeBuilderExtensions.ToQuery(
                modelBuilder.Entity<Abstract>().HasNoKey(), () => context.Set<Abstract>());
            InMemoryEntityTypeBuilderExtensions.ToQuery(
                modelBuilder.Entity<Generic<int>>(), () => context.Set<Generic<int>>());

            VerifyError(
                CoreStrings.DerivedTypeDefiningQuery("Generic<int>", nameof(Abstract)),
                modelBuilder.Model);
        }

        protected override TestHelpers TestHelpers => InMemoryTestHelpers.Instance;
    }
}
