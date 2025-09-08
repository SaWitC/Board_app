import { TaskPriority } from "./enums/task-priority.enum";
import { TaskStatus } from "./enums/task-status.enum";

export interface Task {
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
  status?: any; //TODO: REMOVE
}
