import { TaskPriority } from "./enums/task-priority.enum";
import { TaskType } from "./enums/task-type.enum";

export interface BoardItem {
  id: string;
  title: string;
  description?: string;
  columnId: string;
  priority: TaskPriority;
  assignee?: string;
  createdAt: Date;
  updatedAt: Date;
  dueDate?: Date;
  tags?: string[];
  taskType: TaskType;
}
