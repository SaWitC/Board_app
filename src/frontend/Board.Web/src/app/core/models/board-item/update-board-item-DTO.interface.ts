import { TaskPriority } from "../enums/task-priority.enum";
import { TaskType } from "../enums/task-type.enum";

export interface UpdateBoardItemDTO {
  id: string;
  title: string;
  description: string;
  boardColumnId: string;
  priority: TaskPriority;
  assigneeId: string;
  dueDate: string;
  taskType: TaskType;
}
