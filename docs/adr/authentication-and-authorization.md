# Cookie-based authentication with ASP.NET Core Identity

## Status

Accepted

## Context

Repframe is a first-party React web application with one ASP.NET Core API.
The application stores private training and health-related user data.

## Decision

Use ASP.NET Core Identity with PostgreSQL and cookie-based authentication.
Deploy the web application and API under the same site origin where possible.

Use resource-based authorization: every user-owned query and mutation must
filter or validate ownership using the authenticated user ID.

## Consequences

- Session cookies are HttpOnly, Secure, and SameSite=Lax.
- State-changing endpoints use antiforgery protection.
- JWT and refresh-token management are deferred.
- Identity provides password hashing and account management primitives.
- Future mobile or third-party clients may introduce OAuth/OIDC and JWT
  without replacing the core ownership model.
