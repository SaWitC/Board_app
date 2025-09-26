import { TaskPriority } from "../enums/task-priority.enum";
import { TaskType } from "../enums/task-type.enum";
import { TagDTO } from "../tag/tag-DTO.interface";

export interface UpdateBoardItemDTO {
  id: string;
  title: string;
  description: string;
  boardColumnId: string;
  priority: TaskPriority;
  assigneeEmail?: string|null;
  dueDate: Date;
  taskType: TaskType;
  tags?: TagDTO[];
}
