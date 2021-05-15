import { UserModel } from "./UserModel";

export interface ConnectedUserModel{

   user:UserModel;
   connectionId:string;
}