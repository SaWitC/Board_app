import { UserLookupDTO } from '../user/user-lookup-DTO.model';
export interface BoardDetailsDTO {
  id: string;
  title: string;
  description: string;
  boardUsers: UserLookupDTO[];
  boardColumns: { id?: string; title: string; description: string; }[];
  modificationDate: Date;
  IsTemplate: boolean;
  IsActiveTemplate: boolean;
}
