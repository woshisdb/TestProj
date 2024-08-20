import requests

def read_file(file_path):
    """Read the content of a file and return it as a string."""
    with open(file_path, 'r') as file:
        return file.read()

def send_post_request(url, data):
    """Send a POST request to the specified URL with the given data."""
    response = requests.post(url, data=data)
    return response

def main():
    # Define file paths
    file_a_path = 'domain.pddl'
    file_b_path = 'problem.pddl'
    
    # Read content from files
    content_a = read_file(file_a_path)
    content_b = read_file(file_b_path)
    
    # Define the URL of the Flask endpoint
    url = 'http://localhost:8080/'
    
    # Prepare the data to send in the POST request
    data = {
        'x': content_a,
        'y': content_b
    }
    
    # Send POST request
    response = send_post_request(url, data)
    
    # Print response from the server
    print("Response Status Code:", response.status_code)
    print("Response Content:", response.text)

if __name__ == '__main__':
    main()
