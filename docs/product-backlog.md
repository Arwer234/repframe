# Product Backlog — Repframe

> Backlog is ordered by **dependency flow** and **user value delivery**. Each section builds on the previous one.

---

## 1. Exercises and Plans (Foundation)

> Prerequisite for all other features. Without exercises, there's nothing to log.

### Exercise library

As a trainee, I want to browse an exercise library so that I can choose exercises for sessions.

As a trainee, I want to create my own exercise so that the tracker supports specific machines, movement variants, and equipment from my gym.

> ⏸️ Deferred: `muscleGroup` — see work-methodology.md decision delays table. Added only when volume-by-body-part analysis is needed.

---

## 2. Logging Sets (Core Mechanic)

> **Highest priority.** This is the first feature that delivers real value to a trainee during an actual workout. Everything else depends on having logged sets.

### Basic set logging

As a trainee, I want to add a set with weight, number of repetitions, and RIR so that I can record data necessary for progress analysis and intensity tracking.

As a trainee, I want to edit or delete incorrectly recorded sets so that history is not distorted.

> ✅ `SetType` enum (Working / WarmUp / TopSet / BackOff) already exists in the model — needs UI integration.

### Quick logging aids

As a trainee, I want to copy the previous set with one click so that I can log subsequent sets quickly.

As a trainee, I want to see the result from my last workout for an exercise so that I can easily choose the weight and apply progression.

> 🔗 Depends on: Exercises (to have something to compare against). No dependency on Sessions or Templates.

### Rest timer

As a trainee, I want a rest timer after saving a set so that I don't need to switch between apps.

> 🖥️ Frontend feature — needs `CreatedAt` timestamp from backend (already present in Set model).

---

## 3. Training Sessions (Container)

> Organizes sets into meaningful workout units. The "template" story depends on Exercises (#1), but the "empty session" story can start immediately after Sets (#2).

### Empty sessions

As a trainee, I want to start an empty session so that I can log a spontaneous workout or changes from the plan.

As a trainee, I want to see the current session duration so that I can control whether the workout is not extending excessively.

As a trainee, I want to finish a workout and save a note so that I can preserve context, e.g., poor sleep, equipment limitations, or pain.

As a trainee, I want to return to an unfinished session after refreshing the page so that I don't lose data during a workout.

### Template-based sessions (deferred)

> ⏸️ Requires Exercises and Plans (#1) to exist first — templates cannot be created without exercises.

As a trainee, I want to start a workout from a chosen template so that I have an immediately available list of planned exercises and don't have to manually replay the routine.

---

## 4. Exercise Progression (Analysis Layer)

> Requires logged data from Sets (#2) and Sessions (#3). Cannot exist without historical set records.

### History and comparison

As a trainee, I want to see the set history for a chosen exercise so that I can compare load, repetitions, and RIR across sessions.

As a trainee, I want to compare the current workout with my previous performance of the same exercise so that I can make progression decisions without manually reviewing history.

### Metrics and records

As a trainee, I want to see e1RM trends so that I can track strength changes despite varying repetition counts.

As a trainee, I want to see personal records, e.g., heaviest weight, highest e1RM, and max reps at a given weight.

### Filtering

As a trainee, I want to filter data by period, plan, and exercise variant so that I don't compare mismatched data.

---

## 5. Volume and Fatigue (Advanced Analytics)

> Depends on Progression (#4). Requires significant historical data to be meaningful.

### Volume tracking

As a trainee, I want to see weekly working sets per body part so that I can control volume and not neglect undertrained areas.

> ⏸️ `muscleGroup` must be implemented first (see decision delays table).

As a trainee, I want to see tonnage per exercise and muscle group so that I can assess load trends.

As a trainee, I want to compare the current week to the average of the previous 4 weeks so that I can catch sudden drops or spikes in load.

### Check-ins and alerts

As a trainee, I want to log subjective fatigue, sleep quality, and pain/discomfort after workouts so that I don't interpret performance drops solely through the lens of the program.

As a trainee, I want a non-invasive alert when my exercise score drops over several exposures at similar RIR or when weekly load spikes sharply.
