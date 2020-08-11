// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Protos/TokenService.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Shares.Core.Services {
  public static partial class TokenService
  {
    static readonly string __ServiceName = "Shares.Core.Services.TokenService";

    static readonly grpc::Marshaller<global::Shares.Core.Services.UserCredential> __Marshaller_Shares_Core_Services_UserCredential = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Shares.Core.Services.UserCredential.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Shares.Core.Services.GetTokenResponse> __Marshaller_Shares_Core_Services_GetTokenResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Shares.Core.Services.GetTokenResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Shares.Core.Services.VerifyTokenRequest> __Marshaller_Shares_Core_Services_VerifyTokenRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Shares.Core.Services.VerifyTokenRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Shares.Core.Services.VerifyTokenResponse> __Marshaller_Shares_Core_Services_VerifyTokenResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Shares.Core.Services.VerifyTokenResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::Shares.Core.Services.UserCredential, global::Shares.Core.Services.GetTokenResponse> __Method_Get = new grpc::Method<global::Shares.Core.Services.UserCredential, global::Shares.Core.Services.GetTokenResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Get",
        __Marshaller_Shares_Core_Services_UserCredential,
        __Marshaller_Shares_Core_Services_GetTokenResponse);

    static readonly grpc::Method<global::Shares.Core.Services.VerifyTokenRequest, global::Shares.Core.Services.VerifyTokenResponse> __Method_Verify = new grpc::Method<global::Shares.Core.Services.VerifyTokenRequest, global::Shares.Core.Services.VerifyTokenResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Verify",
        __Marshaller_Shares_Core_Services_VerifyTokenRequest,
        __Marshaller_Shares_Core_Services_VerifyTokenResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Shares.Core.Services.TokenServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of TokenService</summary>
    [grpc::BindServiceMethod(typeof(TokenService), "BindService")]
    public abstract partial class TokenServiceBase
    {
      public virtual global::System.Threading.Tasks.Task<global::Shares.Core.Services.GetTokenResponse> Get(global::Shares.Core.Services.UserCredential request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      public virtual global::System.Threading.Tasks.Task<global::Shares.Core.Services.VerifyTokenResponse> Verify(global::Shares.Core.Services.VerifyTokenRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for TokenService</summary>
    public partial class TokenServiceClient : grpc::ClientBase<TokenServiceClient>
    {
      /// <summary>Creates a new client for TokenService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public TokenServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for TokenService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public TokenServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected TokenServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected TokenServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      public virtual global::Shares.Core.Services.GetTokenResponse Get(global::Shares.Core.Services.UserCredential request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Get(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Shares.Core.Services.GetTokenResponse Get(global::Shares.Core.Services.UserCredential request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Get, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Shares.Core.Services.GetTokenResponse> GetAsync(global::Shares.Core.Services.UserCredential request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Shares.Core.Services.GetTokenResponse> GetAsync(global::Shares.Core.Services.UserCredential request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Get, null, options, request);
      }
      public virtual global::Shares.Core.Services.VerifyTokenResponse Verify(global::Shares.Core.Services.VerifyTokenRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Verify(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual global::Shares.Core.Services.VerifyTokenResponse Verify(global::Shares.Core.Services.VerifyTokenRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Verify, null, options, request);
      }
      public virtual grpc::AsyncUnaryCall<global::Shares.Core.Services.VerifyTokenResponse> VerifyAsync(global::Shares.Core.Services.VerifyTokenRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return VerifyAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      public virtual grpc::AsyncUnaryCall<global::Shares.Core.Services.VerifyTokenResponse> VerifyAsync(global::Shares.Core.Services.VerifyTokenRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Verify, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override TokenServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new TokenServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(TokenServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_Get, serviceImpl.Get)
          .AddMethod(__Method_Verify, serviceImpl.Verify).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, TokenServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_Get, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Shares.Core.Services.UserCredential, global::Shares.Core.Services.GetTokenResponse>(serviceImpl.Get));
      serviceBinder.AddMethod(__Method_Verify, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Shares.Core.Services.VerifyTokenRequest, global::Shares.Core.Services.VerifyTokenResponse>(serviceImpl.Verify));
    }

  }
}
#endregion
