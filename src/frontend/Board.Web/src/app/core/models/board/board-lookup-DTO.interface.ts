export interface BoardLookupDTO {
  id: string;
  title: string;
  description: string;
  boardUsers?: { email: string; role: number; }[];
  modificationDate?: Date;
  IsTemplate: boolean;
  IsActiveTemplate: boolean;
}
