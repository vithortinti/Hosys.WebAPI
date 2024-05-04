from pdf2image import convert_from_path
import sys

def main():
    pdf = sys.argv[1]
    image_directory = sys.argv[2]
    execution_type = sys.argv[3]
    
    if execution_type == "image":
        path = convert_to_image(pdf, image_directory)
        print(path)

def convert_to_image(pdf_file: str, image_path: str) -> str:
    images = convert_from_path(pdf_file)
    for i, image in enumerate(images):
        image.save(f"{image_path}/page_{i+1}.jpg", "JPEG")
    return image_path

if __name__ == "__main__":
    main()