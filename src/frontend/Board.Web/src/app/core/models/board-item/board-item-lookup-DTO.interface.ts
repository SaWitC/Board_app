import { TaskPriority } from "../enums/task-priority.enum";
import { TaskType } from "../enums/task-type.enum";

export interface BoardItemLookupDTO {
  id: string;
  title: string;
  boardColumnId?: string;
  taskType: TaskType;
  priority: TaskPriority;
  dueDate:Date|null;
  tags?: string[];
  assignee:string;
}

