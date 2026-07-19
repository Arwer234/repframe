# Slice 001 — Log workout set

## Goal

A trainee can log a working set for an exercise during an active
workout session, and the data persists after page refresh.

## User story

As a trainee, I want to add a working set with weight, repetitions,
and RIR so that I can record data necessary for progress analysis
and intensity tracking.

## Context and assumptions

- The application has one seeded active workout session.
- The exercise list is seeded and read-only (see Seed data below).
- The set type is always `working`.
- Authentication is not part of this slice.
- The user may select an exercise from the seeded list.
- Weight can be entered in kilograms (default) or pounds; conversion happens on the client.

## Data models

### WorkoutSession

| Field     | Type                         | Notes                                      |
| --------- | ---------------------------- | ------------------------------------------ |
| Id        | Guid                         | Primary key                                |
| Status    | Enum (`active`, `completed`) | Only one session can be `active` at a time |
| StartedAt | DateTimeOffset UTC           | When the session was created/started       |

### Exercise

| Field       | Type                       | Notes                            |
| ----------- | -------------------------- | -------------------------------- |
| Id          | Guid                       | Primary key                      |
| Name        | string (required, max 100) | Display name, e.g. "Bench Press" |
| MuscleGroup | string (optional, max 50)  | e.g. "Chest", "Back", "Legs"     |

### WorkoutSet

| Field       | Type                           | Notes                                                  |
| ----------- | ------------------------------ | ------------------------------------------------------ |
| Id          | Guid                           | Primary key                                            |
| ExerciseId  | Guid (FK → Exercise)           | Which exercise this set belongs to                     |
| WeightValue | decimal (required, > 0, ≤ 500) | Numeric weight value — unit depends on user preference |
| WeightUnit  | Enum (`kg`, `lb`)              | Stored as entered; client converts for display         |
| Reps        | int (required, 1–100)          | Number of completed repetitions                        |
| Rir         | byte (required, 0–10)          | Reps in reserve                                        |
| CreatedAt   | DateTimeOffset UTC             | When the set was logged                                |

## Seed data

### Active session

```json
{ "id": "<seeded-active-session>", "status": "active", "startedAt": "<now>" }
```

### Exercises (minimum 5)

| Id               | Name           | MuscleGroup |
| ---------------- | -------------- | ----------- |
| seed-bench-press | Bench Press    | Chest       |
| seed-squat       | Squat          | Legs        |
| seed-deadlift    | Deadlift       | Back / Legs |
| seed-ohp         | Overhead Press | Shoulders   |
| seed-barbell-row | Barbell Row    | Back        |

## API contract

### GET `/api/workout-sessions/active`

Returns the single active workout session, or `404 Not Found` if none exists (should not happen with seeding).

**200 OK**

```json
{ "id": "<guid>", "status": "active", "startedAt": "2026-07-19T08:00:00Z" }
```

### GET `/api/exercises`

Returns the seeded, read-only exercise list. No auth required.

**200 OK**

```json
[
  { "id": "<guid>", "name": "Bench Press", "muscleGroup": "Chest" },
  ...
]
```

### POST `/api/workout-sessions/{sessionId}/sets`

Logs a new working set. Returns `400 Bad Request` with validation errors if input is invalid.

**Request body**

```json
{ "exerciseId": "<guid>", "weightValue": 100, "weightUnit": "kg", "reps": 8, "rir": 2 }
```

**201 Created**

```json
{
	"id": "<guid>",
	"exerciseId": "<guid>",
	"weightValue": 100,
	"weightUnit": "kg",
	"reps": 8,
	"rir": 2,
	"createdAt": "2026-07-19T08:05:00Z"
}
```

**400 Bad Request**

```json
{ "errors": [ { "field": "weightValue", "message": "Weight must be between 0.1 and 500." }, ... ] }
```

### GET `/api/workout-sessions/{sessionId}/sets`

Returns all sets logged in the session, ordered by `createdAt` ascending.

**200 OK**

```json
[
  { "id": "<guid>", "exerciseId": "<guid>", "weightValue": 100, "weightUnit": "kg", "reps": 8, "rir": 2, "createdAt": "2026-07-19T08:05:00Z" },
  ...
]
```

## Acceptance criteria

- Given an active workout session, I can select an exercise.
- I can enter weight in kilograms, repetitions, and RIR.
- Weight must be greater than 0 and no greater than 500 kg (or equivalent in other units).
- Repetitions must be an integer from 1 to 100.
- RIR must be an integer from 0 to 10.
- Invalid fields show an accessible validation message.
- After submitting valid data, I see the logged set in the current session.
- The set remains visible after refreshing the page.
- If saving fails, I see an error and entered form values remain intact.

## API contract

- `GET /api/workout-sessions/active`
- `GET /api/exercises`
- `POST /api/workout-sessions/{sessionId}/sets`
- `GET /api/workout-sessions/{sessionId}/sets`

## Out of scope

- Authentication and multiple users
- Starting or finishing sessions
- Workout templates
- Creating/editing exercises
- Editing or deleting a set
- Warm-up, top, back-off, and drop sets
- Timer
- History, e1RM, PRs, volume, charts, alerts
- Docker, Redis, events, SignalR, AI
