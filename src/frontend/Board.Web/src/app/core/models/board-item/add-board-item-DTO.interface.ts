import { TaskPriority } from "../enums/task-priority.enum";
import { TaskType } from "../enums/task-type.enum";

export interface AddBoardItemDTO {
  title: string;
  description: string;
  boardColumnId: string;
  priority: TaskPriority;
  assignee: string;
  dueDate: Date|null;
  taskType: TaskType;
  tags?: string[];
}
