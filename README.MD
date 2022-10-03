## Ocelot demo with .net 6

API Gateway is an API management tool that usually sits between the external caller (Web or Mobile) and the internal services. The API Gateway can provide multiple features like:

1. Routing
2. Request Aggregation
3. Authentication
4. Authorization
5. Rate Limiting
6. Caching
7. Load Balancing
8. More anything... 

Ocelot is an ASP.Net Core (Supports .Net Core 6) API Gateway. It's a NuGet package, which can be added to any ASP.Net Core application to make it an API Gateway.
Ocelot API Gateway supports all the features that any standard API Gateway does.

I will cover the following features of Ocelot API Gateway in this sample:

1. Routing (Basic routing to internal service)
2. Authentication (JWT Token-based authentication)
3. Response Caching (Using Ocelot.Cache.CacheManager NuGet package)
4. Rate limiting
5. Custom Load Balancers

## More

More information about Ocelot API Gateway is available in their website here: https://ocelot.readthedocs.io/en/late...

## Project structure overview

1. API Gateway Ocelot.demo project 

- baseUrl:	https://localhost:5021/api/weather

2. Demo API Node 1

- baseUrl:	https://localhost:5001

3. Demo API Node 2

- baseUrl:	https://localhost:5011

4. Demo JWTAuth.WebAPI JWT token-based authentiaction
    
- baseUrl: https://localhost:7067/api/token
- Request body:
```
	{
    "username": "admin",
    "password": "admin"
	}
```