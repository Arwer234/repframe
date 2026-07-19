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
- The exercise list is seeded and read-only.
- The set type is always `working`.
- Authentication is not part of this slice.
- The user may select an exercise from the seeded list.

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
