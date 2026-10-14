namespace GymTracker.Application.Workouts.UpdateWorkout;

public record UpdateWorkoutRequest(Guid WorkoutId, UpdateWorkoutDto Dto );