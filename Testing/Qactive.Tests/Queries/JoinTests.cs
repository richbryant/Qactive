﻿using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.Reactive.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Qactive.Tests
{
  [TestClass]
  public class JoinTests : TestBase
  {
    [TestMethod]
    public async Task JoinClosure()
    {
      var service = TestService.Create(QbservableServiceOptions.Unrestricted, Observable.Return(new TestContext()));
      var range3To7 = Observable.Range(3, 5);

      var results = await service.QueryAsync(source => from context in source
                                                       from x in context.Range0To5
                                                       join y in range3To7
                                                       on Observable.Never<Unit>() equals Observable.Never<Unit>()
                                                       where x == y
                                                       select x + y);

      results.AssertEqual(OnNext(6), OnNext(8), OnNext(10), OnCompleted<int>());
    }

    [TestMethod]
    public async Task JoinInternal()
    {
      var service = TestService.Create(QbservableServiceOptions.Unrestricted, Observable.Return(new TestContext()));

      var results = await service.QueryAsync(source => from context in source
                                                       from x in context.Range0To5
                                                       join y in Observable.Range(3, 7)
                                                       on Observable.Never<Unit>() equals Observable.Never<Unit>()
                                                       where x == y
                                                       select x + y);

      results.AssertEqual(OnNext(6), OnNext(8), OnNext(10), OnCompleted<int>());
    }

    private sealed class TestContext
    {
      public IObservable<int> Range0To5 => Observable.Range(0, 6);
    }
  }
}
