namespace NaviriaAPI.DTOs.Task.Update
{
    public class TaskWithSubtasksUpdateDto : TaskUpdateDto
    {
        public TaskWithSubtasksUpdateDto()
        {
            Type = "with_subtasks";
        }
    }
}
