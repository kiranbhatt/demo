import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Member } from '../_models/member';

@Injectable({
  providedIn: 'root'
})
export class MembersService {

  baseUrl = environment.baseUrl;

  members: Member[] = [];

  constructor(private http: HttpClient) { }

  getMembers() {
    // of method convert it to Observable
    if (this.members.length > 0) return of(this.members);

    return this.http.get<Member[]>(this.baseUrl + "v1/users/").pipe(
      map(members => {

        this.members = members;
        return this.members;
      }))
  }

  getMember(username: string) {

    const member = this.members.find(member => member.name === username);
    if (member) return of(member);

    return this.http.get<Member>(this.baseUrl + "v1/users/" + username)
  }

  updateMember(model: Member) {

    return this.http.put(this.baseUrl + "v1/users", model);
  }

}
