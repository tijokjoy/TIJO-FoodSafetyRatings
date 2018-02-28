import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'fetchdata',
    templateUrl: './fetchdata.component.html'
})
export class FetchDataComponent {
    public localAuthorities: Authorities[];
    public ratingOverview: RatingOverview[];
    public baseURL: string;
    public http: Http;
   

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.http=http;
        this.baseURL = baseUrl;
        
        http.get(baseUrl + 'api/FSAData/GetAuthorties').subscribe(result => {
            this.localAuthorities = result.json() as Authorities[];
            this.onSelect(this.localAuthorities[0].LocalAuthorityId);
        }, error => console.error(error));

        
    };

    onSelect(localAuthorityId: number) {
        this.ratingOverview = [];
        this.http.get(this.baseURL + 'api/FSAData/GetEstablishmentRatings?localAuthorityId=' + localAuthorityId).subscribe(result => {
            this.ratingOverview = result.json() as RatingOverview[];
        }, error => console.error(error));
    }
}

interface Authorities {
    Name: string;
    LocalAuthorityId: number;
    LocalAuthorityIdCode: string;
    EstablishmentCount: number    
}
interface RatingOverview {
    Rating: string;
    Percentage: number
}
