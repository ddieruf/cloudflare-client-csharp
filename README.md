# cloudflare-client-csharp

This is a C# client of the endpoints that I needed for v4 of the [CloudFlare api](https://api.cloudflare.com/).

## Endpoints Implemented

User ([CloudFlare ref](https://api.cloudflare.com/#user-properties)): Get Details, Update

Zone ([CloudFlare ref](https://api.cloudflare.com/#zone-properties)): Create, Get Details, Delete, List

Load Balancer ([CloudFlare ref](https://api.cloudflare.com/#load-balancers-properties)): Create, Get Details, Delete, List

Load Balancer Pool ([CloudFlare ref](https://api.cloudflare.com/#load-balancer-pools-properties)): Create, Get Details, Delete, List, Add Origin, Delete Origin, Set Minimum Origins

Load Balancer Monitor ([CloudFlare ref](https://api.cloudflare.com/#load-balancer-monitors-properties)): Create, Get Details, Delete, List

## Get Started

To use this library first add it to your C# project

```bash
$ dotnet add package xxxxxxx
```

Set your CloudFlare API credentials. Refer to the [API keys area of the CloudFlare docs](https://api.cloudflare.com/#getting-started-requests) for more info.

```bash
var _xAuthEmail = "<YOUR_EMAIL>";
var _xAuthKey = "<YOUR_KEY>";
```

Create an instance of the client and construct the desired endpoint class.

```c#
var apiClient = new ApiClient(new Configuration(_xAuthEmail,_xAuthKey));
var loadBalancerPoolApi = new LoadBalancerPoolApi(apiClient);
```

Make a call to an endpoint

```c#
var loadBalancerPools = await loadBalancerPoolApi.ListLoadBalancerPoolsAsync();
```

## Features

The client uses [RestSharp](https://restsharp.dev/) for all HTTP interactions. The design has been influenced by the patterns in [openapi-generator C# template](https://github.com/OpenAPITools/openapi-generator/tree/master/modules/openapi-generator/src/main/resources/csharp-netcore) as well as the [kubernetes C# client](https://github.com/kubernetes-client/csharp).

Along with the obvious crud actions the load balancer pool object has some added benefits.
1. Re-balance origin weight: when an origin is added/removed, equal weight can be (optionally) recalculated between all of them.
2. Logical pool validation: when any change or additions are made to a pool, they are validated for logic. Things like is there at least 1 origin, and is the minimum origin equal to or less than the origin count.

Unless overridden, the client will assume a base path of `https://api.cloudflare.com/client/v4`.
