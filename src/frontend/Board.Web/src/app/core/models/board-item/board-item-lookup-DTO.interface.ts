import { TaskPriority } from "../enums/task-priority.enum";
import { TaskType } from "../enums/task-type.enum";
import { TagDTO } from "../tag/tag-DTO.interface";

export interface BoardItemLookupDTO {
  id: string;
  title: string;
  boardColumnId?: string;
  taskType: TaskType;
  priority: TaskPriority;
  dueDate:Date|null;
  tags?: TagDTO[];
  assigneeEmail:string|null;
}

