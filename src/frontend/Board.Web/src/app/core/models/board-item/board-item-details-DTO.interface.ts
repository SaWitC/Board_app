export interface BoardItemDetailsDTO {
  id: string;
  title: string;
  description: string;
  boardColumnId: string;
  priority: number;
  assigneeId: string;
  dueDate: string;
  modificationDate?: string;
  createdTime?: string;
}
